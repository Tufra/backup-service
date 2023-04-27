terraform {
  required_providers {
    yandex = {
      source = "yandex-cloud/yandex"
    }
  }
  required_version = ">= 0.13"
}

variable "cloud_id" {
  description = "cloud id"
  sensitive = true
}
variable "folder_id" {
  description = "folder id"
  sensitive = true
}

variable "subnet_id" {
  description = "subnet id"
  sensitive = true
}

variable "ssh_key" {
  description = "public ssh key"
  sensitive = true
}

variable "image_tag" {
  description = "container image tag"
  sensitive = true
}

variable "sa_key" {
  description = "service account key json"
  sensitive = true
}

provider "yandex" {
  zone = "ru-central1-b"
  service_account_key_file = var.sa_key
  cloud_id                 = var.cloud_id 
  folder_id                = var.folder_id
}

resource "yandex_compute_instance" "main_vm" {
  name = "terraform1"
  platform_id = "standard-v2"

  boot_disk {
    initialize_params {
      image_id = data.yandex_compute_image.container-optimized-image.id
    }
  }

  resources {
    memory = 2
    cores = 2
    core_fraction = 5
  }

  network_interface {
    subnet_id = var.subnet_id
    nat = true
  }

  scheduling_policy {
    preemptible = true
  }

  metadata = {
    docker-compose = format(file("${path.module}/docker-compose.yaml"), var.image_tag)
    user-data = format(file("${path.module}/cloud-config.yaml"), var.ssh_key)
  }
}

data "yandex_compute_image" "container-optimized-image" {
  family = "container-optimized-image"
}

output "external_ip" {
  value = yandex_compute_instance.main_vm.network_interface.0.nat_ip_address
}