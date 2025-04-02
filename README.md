# CLIW 
Console line interface weather app, fetching weather information from WeatherStack Free API.

## Requirements
- **DOTNET 9.0**: Usually comes with your IDE. Or install it manually.
   
  On mac:
  ``` zsh
  brew install --cask dotnet
  ```

  On Windows:
  [download](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
  
- [**DotNetEnv nuget package**](https://www.nuget.org/packages/DotNetEnv):
  Enter the command below into the terminal in your project folder.
  
  ``` zsh
  dotnet add package DotNetEnv
  ```

## API key

Register on [weatherstack.com](https://weatherstack.com/) to get your API key.

Create a **.env** file into **/your_project/bin/Debug/net9.0** folder and put your API Key into it.  

``` .env
API_KEY=your_api_key
```

## Key features
Display detailed weather report on your console.
- location
- weather information
- astro information
- forecast (soon)

## Run
To run the project enter the command below into your terminal:
``` zsh
dotnet run
```
If you don't enter any location, then **Budapest** will be set as default. 
Soon you will be able to give your location as an argument. Example:  
``` zsh
dotnet run -- "London"
```

or enter the country too:
``` zsh
dotnet run -- "London, United Kingdom"
```
