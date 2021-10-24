!!!! Uncomment Ignored lines in BiletallContext before creating a migration

add-migration initial -context BiletallContext -o EntityFramework/Migrations

update-database -context BiletallContext

remove-migration -context BiletallContext 