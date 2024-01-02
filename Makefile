.PHONY: build infra run clean

build-api:
	@docker-compose up api --build --no-start

run-api: build infra
	@docker-compose up -d api

build-service:
	@docker-compose up service --build --no-start

run-service: build infra
	@docker-compose up -d service

infra:
	@docker-compose up -d rabbitmq

clean:
	@docker-compose down