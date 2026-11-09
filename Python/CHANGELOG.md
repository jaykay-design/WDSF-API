# Changelog

## [0.2.0]

### Changed
- Authenticates with an API key (`WdsfHttpClientConfig.api_key`, sent as `X-WDSF-API-Key`) instead
  of a user name and password; `BasicAuthCredentials` is gone and `HttpClient` takes `api_key`
- Targets the WDSF API v2: use `https://api.wdsf.org/api/2/` as the base URL
  (sandbox: `https://api-staging.wdsf.org/api/2/`)
- `CompetitionStatus` and `CompetitionDivision` hold every value v2 returns (e.g. `InProgress`,
  `Postponed`, `Breaking`); competitions in those states no longer fail validation

### Added
- `Competition`: discipline, dance form, age, coefficient, date, status, division, country,
  location and competition type (all optional)
- `CompetitionDetails`: name, dance form, event id and group id (optional)
- `CompetitionQuery`: `age_group`, `group_id`, `page`, `page_size`

### Fixed
- `HttpClient.build_url` joined paths with `\` on Windows

## [0.1.0]

### Added
- HttpClient with configurable timeout and basic authentication
- CompetitionProvider for querying competitions with filters
- CompetitionDetailsProvider for fetching individual competition details
- Configuration management via environment variables
