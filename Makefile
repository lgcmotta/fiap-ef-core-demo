#!make

add:
	dotnet ef migrations add InitialModeling --project ./src/BankingApp.API --startup-project ./src/BankingApp.API --context AccountsDbContext 

update:
	dotnet ef database update --verbose --project ./src/BankingApp.API --startup-project ./src/BankingApp.API --context AccountsDbContext

remove:
	rm -rf ./src/BankingApp.API/Migrations

up:
	docker compose up -d

down:
	docker compose down

restart:
	docker compose down && docker volume rm $$(docker volume ls -q) && docker compose up -d