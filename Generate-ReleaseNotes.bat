rem https://github.com/StefH/GitHubReleaseNotes

SET version=0.10.0.2

GitHubReleaseNotes --output ReleaseNotes.md --skip-empty-releases --exclude-labels question invalid documentation --version %version%

GitHubReleaseNotes --output PackageReleaseNotes.txt --skip-empty-releases --exclude-labels question invalid documentation --template PackageReleaseNotes.template --version %version%