
# Change Log
All notable changes to this project will be documented in this file.

## [2.1.3-preview] - 2022-11-06
  
### Added

### Changed
- Handler.cs Try Parse for new timestamp default configuration. [Commit Trace](https://github.com/duccl/cache-it/commit/d8282b8b45b59908445c6472fdc99fa0fddf6e7f)
- CustomRefreshOptions.cs using TimeStamp instead of double  [Commit Trace](https://github.com/duccl/cache-it/commit/50057a553d2d300252504d3f083a6a2766cbbc6b)
- Handles.cs correct usage of IOptionsMonitor [Commit Trace](https://github.com/duccl/cache-it/commit/50057a553d2d300252504d3f083a6a2766cbbc6b)
- Readme Description for the new custom configuration. [Commit Trace](https://github.com/duccl/cache-it/commit/4c6ce18c0c9b4298dded541f4fde156e49096adc)
 
### Fixed

## [1.1.3] - 2022-07-17

### Added
- Version [1.1.3-preview](#113-preview---2022-05-25) as official
### Changed

### Fixed

## [1.1.3-preview] - 2022-05-25
  
### Added
- Custom config for refresh the cacheable componenets. [Commit Trace](https://github.com/duccl/cache-it/commit/bf822009973781f50e56f447bc067a96ec51a118)
- More tests to validate the new version, with services registered interface and without. [Commit Trace](https://github.com/duccl/cache-it/commit/faf908d1aab2deb0b78239a941416c485efa2849)
- New overloaded AddCacheIt method to configure the custom configs. [Commit Trace](https://github.com/duccl/cache-it/commit/72237a869bac129a4674aef5c52b93f07250c22f)

### Changed
- AddCacheIt extension. [Commit Trace](https://github.com/duccl/cache-it/commit/72237a869bac129a4674aef5c52b93f07250c22f)
- Handler.cs Injection. [Commit Trace](https://github.com/duccl/cache-it/commit/bf822009973781f50e56f447bc067a96ec51a118)
- Handles.cs lists with components to refresh. Using HashSet's instead of IEnumerable. [Commit Trace](https://github.com/duccl/cache-it/commit/bf822009973781f50e56f447bc067a96ec51a118)
- Readme Description for the custom configuration. [Commit Trace](https://github.com/duccl/cache-it/commit/46fa6f31da1b88dd357afdc700a0bd76942ecd21)
 
### Fixed

## [1.0.3] - 2022-05-21
  
### Added
- Version [1.0.3-preview](#103---2022-05-13) as official

### Changed
 
### Fixed

## [1.0.3-preview] - 2022-05-13
  
### Added
- Support for decimal minutes at appsettings parse at [Handler.cs](/src/CacheIt/Hosting/Handler.cs). Made via commit [adding support to dobule numbers at config](https://github.com/duccl/cache-it/commit/5844bc405ce87e22dc2db42602f5c9302d8e02bc)

### Changed
 
### Fixed
- [VERSION 1.0.2](#102---2021-12-06) error when minutes with decimal place is added at appsettings. Fix made via commit [adding support to dobule numbers at config](https://github.com/duccl/cache-it/commit/5844bc405ce87e22dc2db42602f5c9302d8e02bc)

## [1.0.2] - 2021-12-06
  
### Added
- Refreshing Components Registered via the Interface at IServiceCollection
- Test with Interface Injected

### Changed
 
### Fixed
 
## [1.0.1] - 2021-12-06
 
### Added
- Package Icon
   
### Changed
 
### Fixed

## [1.0.0] - 2021-12-02
 
### Added
- Project CacheIt first version
   
### Changed
 
### Fixed
