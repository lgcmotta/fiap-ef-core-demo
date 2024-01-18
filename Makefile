#!make

add-bad:
	dotnet ef migrations add InitialModeling --project ./examples/EFCoreDemo.BadExamples --context BloggingContext

update-bad:
	dotnet ef database update --project ./examples/EFCoreDemo.BadExamples --context BloggingContext --connection "Data Source=blogging.db"

add:
	dotnet ef migrations add InitialModeling --project ./src/BankingApp.API --startup-project ./src/BankingApp.API --context AccountsDbContext --connection "Data Source=localhost;Initial Catalog=BankingApp;Persist Security Info=True;User ID=root;Password=123456;"

update:
	dotnet ef database update  --project ./src/BankingApp.API --startup-project ./src/BankingApp.API --context AccountsDbContext --connection "Data Source=localhost;Initial Catalog=BankingApp;Persist Security Info=True;User ID=root;Password=123456;"

remove:
	rm -rf ./src/BankingApp.API/Migrations

up:
	docker compose up -d

down:
	docker compose down

restart:
	docker compose down && docker volume rm $$(docker volume ls -q) && docker compose up -d