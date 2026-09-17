SERVER := Shogun.Server

all:
	dotnet run --project $(SERVER)

build:
	dotnet build

test:
	dotnet test

migrate:
	dotnet ef migrations add $(NAME) --project $(SERVER) --startup-project $(SERVER)

db-update:
	dotnet ef database update --project $(SERVER) --startup-project $(SERVER)

jwt-create:
	dotnet user-jwts create --project $(SERVER) -n $(NAME)

jwt-list:
	dotnet user-jwts list --project $(SERVER)

jwt-list-user:
	dotnet user-jwts print $(ID) --project $(SERVER) --show-all

.PHONY: all build test migrate db-update jwt-create jwt-list jwt-list-user
