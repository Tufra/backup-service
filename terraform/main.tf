terraform {
  required_providers {
    yandex = {
      source = "yandex-cloud/yandex"
    }
  }
  required_version = ">= 0.13"
}

provider "yandex" {
  zone = "ru-central1-b"
  service_account_key_file = file("${path.module}/../../../key.json")
  cloud_id                 = "b1gpi61dpuld9gqcsg9m"
  folder_id                = "b1g5qj560obk3rtg0arp"
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
    subnet_id = "e2lmhvjubv98govb13gu"
    nat = true
  }

  scheduling_policy {
    preemptible = true
  }

  metadata = {
    docker-compose = file("${path.module}/docker-compose.yaml")
    user-data = file("${path.module}/cloud-config.yaml")
  }
}

data "yandex_compute_image" "container-optimized-image" {
  family = "container-optimized-image"
}

output "external_ip" {
  value = yandex_compute_instance.main_vm.network_interface.0.nat_ip_address
}