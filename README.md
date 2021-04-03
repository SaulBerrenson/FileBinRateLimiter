# FileBinRateLimiter
Library for reading binary files with rate limit (speed limit by byte to second).
Example need reading file  not more then 1 mb per sec.
Rate limiter was used ported class Google.Guava.RateLimiter

### nuget
https://www.nuget.org/packages/FileRateLimiter/

### Usage
```
  const int SPEED_LIMIT = 1024 * 1024 * 5;
  
  using (ExtentedBinaryReader binaryReader = ExtentedBinaryReader.CreateInstance(new FileStream(pathToFile, FileMode.Open), Encoding.UTF8))
  {
      binaryReader.AddFilter(new FilterRateLimit(SPEED_LIMIT));
      
      byte[] buffer = new byte[buffer_size];
      var maxCount = new FileInfo(pathToFile).Length;
      while (countRead < maxCount)
      {
         var bytes= binaryReader.ReadBytes(buffer_size);
         countRead += bytes.Length;
      }
  }
  
```

