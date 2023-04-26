terraform {
  required_providers {
    yandex = {
      source = "yandex-cloud/yandex"
    }
  }
  required_version = ">= 0.13"
}

# variable "cloud_id" {
#   default = "b1gpi61dpuld9gqcsg9m"
# }
# variable "folder_id" {
#   default = "b1g5qj560obk3rtg0arp"
# }

# variable "subnet_id" {
#   default = "e2lmhvjubv98govb13gu"
# }

# variable "ssh_key" {
#   default = "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIEpMqnaLlaRp4LJS5yyM51OwlBUJNw3nFwHZ21C8XwAP"
# }

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
    docker-compose = file("${path.module}/docker-compose.yaml")
    user-data = format(file("${path.module}/cloud-config.yaml"), var.ssh_key)
  }
}

data "yandex_compute_image" "container-optimized-image" {
  family = "container-optimized-image"
}

output "external_ip" {
  value = yandex_compute_instance.main_vm.network_interface.0.nat_ip_address
}