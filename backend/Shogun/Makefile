all:
		dotnet run

build:
		dotnet build

migrate:
		dotnet ef migrations add $(NAME)

db-update:
		dotnet ef database update

jwt-create:
		dotnet user-jwts create -n $(NAME)
	
jwt-list:
		dotnet user-jwts list

jwt-list-user:
		dotnet user-jwts print $(ID) --show-all