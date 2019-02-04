timestamp() {
  date +"%Y-%m-%d_%H-%M-%S"
}

dotnet ef --project MsvcAuth add migration_$timestamp