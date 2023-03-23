# Run on Development

On development, `ClientApp` and `ServerApp` can be separated running on your machine. When you need to develop `CliantApp`, you can serve only `CliantApp`.

> If not using the `DataParser` in your application, **`ServerApp` is not required running and deploy on web server**.

## ClientApp
1. Open Command Prompt and navigates directory to the `ClientApp` folder.
```cmd
cd ClientApp
```

2. Install packages with following command:
```cmd
npm install
```

3. Your `ClientApp` project is now ready to run. Serve the project with following command:
```cmd
# Use cli from local project.
npx ng serve

# Use cli from global.
ng serve
```

The `npx` makes it easy to use CLI tools and other executables hosted on the registry. It greatly simplifies a number of things that, until now, required a bit of ceremony to do with plain npm.

## ServerApp (Optional)

If your web application can be connected to the web service directly then `ServerApp` is not required. `ServerApp` are required when application need to use the `DataParser`.

1. In the `environment.ts` configure file, change value the `webServiceUrl` property with `ServerApp` url. You will see the url in terminal console when you serve `ServerApp` with `dotnet run` command. After you have to reconfigured `ClientApp` environment then serve its with `npx ng serve` command.
```json
{
  "webServiceUrl": "https://localhost:5003"
}
```

2. From root directory, navigates to `ServerApp` then serve your back-end with `dotnet run` command or use `watch` before `run` for rebuild when files changeed.
```cmd
dotnet watch run
```


# Publish and Deploy on Production

## Without DataParser

Install the `URL rewrite module` from this link - https://www.iis.net/downloads/microsoft/url-rewrite

If the `DataParser` is not used in your application, you can build only `CliantApp`.

```cmd
cd CliantApp
npx ng build --prod
```

Copy all in `dist` folder to your web server.


## With DataParser

```cmd
cd ServerApp
dotnet publish -c Release

cd ../ClientApp
npx ng build --prod --output-path="../ServerApp/bin/Release/netcoreapp3.1/publish/ClientApp"
```

Copy all in `publish` folder to your web server.

See more information about [build options](https://angular.io/cli/build#options).