# dotnet-vision-predict
C# app that calls the Salesforce Einstein Vision API. This app sends an image to the API, receives the prediction response and displays the response in text and in a graph.

## Prerequisites

- Json.NET—Open source JSON Framework available from [Newtonsoft](http://www.newtonsoft.com). To install from VS, go to **Tools** > **NuGet Package Manager** > **Package Manager Console**. At the **PM>** prompt enter `Install-Package Newtonsoft.Json`.

- Einstein Platform account—Go to the [sign-up page](https://api.metamind.io/signup) to sign up using your Salesforce credentials. 

- OAuth access token—Go to the [token page](https://api.metamind.io/token) to get a valid access token. Enter your email address and upload your key found in the `einstein_platform.pem` file.

## Run the App

You must update the variable with your access token to run the solution. In the `btnPredict_Click` event, update this variable:

```var authToken = "replace_with_your_token";```

So that it looks like this but with your token:

```var authToken = "8da2a2b54b04abc1sj39a7fabd160da4fd3df72";```

Run the solution and click **Predict**.

This project was created using Visual Studio 2017 Community Edition. Thanks to Travis Illig for his illuminating [blog post](http://www.paraesthesia.com/archive/2009/12/16/posting-multipartform-data-using-.net-webrequest.aspx/) about multipart/form-data requests. His code to build the multipart request is included in this project. 

## Other Resources

- [Einstein Vision Documentation](https://metamind.readme.io/v1/docs)

- [Trailhead Quick Start: Einstein Vision](https://trailhead.salesforce.com/en/projects/predictive_vision_apex)
