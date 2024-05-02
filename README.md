BE Application Setup Guide

To run the BE Application locally, follow these steps:

    Clone the Repository:
    Clone the project from GitHub using the following link: https://github.com/RamakrishnaMB/WeatherConsoleDemo.git, or alternatively, extract the ZIP file.

    Open in Visual Studio:
    Open the solution file (*.sln) in Visual Studio 2022.

    Run the Project:
    Build and run the project in Visual Studio. Upon successful execution, the main window of the console application will be displayed.

    Exiting the Program:
    To close the program, simply press Ctrl+C.

    Data Storage Paths:
    As the application makes calls to two separate APIs (one for History and one for Forecast), two distinct folders will be created:
        For History data, JSON files will be generated in: bin\Debug\net8.0\weatherhistorydata
        For Forecast data, JSON files will be generated in: bin\Debug\net8.0\weatherforecastdata

    JSON File Naming:
    JSON files will be created for each country. For instance: India.json, Singapore.json, and so forth.


Docker Setup Instructions for BE

To run the application using Docker, follow these steps:

    Navigate to Project Root:
    Navigate to the root directory of the project. You will find two Dockerfiles, one for Windows and another for Linux. Example root path: D:\BE\WeatherConsoleDemo\ThalesGroupDemo

    Locate Dockerfiles:
    Inside this root directory, locate the two Dockerfiles named Dockerfile-linux and Dockerfile-windows.

    Open Terminal or Command Prompt:
    Open a terminal or command prompt and navigate to the root path of the project.

    Ensure Docker Desktop Installation:
    Note: Docker Desktop must be installed on your Windows machine to execute these Dockerfiles.

    Build Docker Image:
    Use the following command to build the Docker image:

docker build . -t weather-app-backend -f Dockerfile-linux

This command will build the Docker image named weather-app-backend using the Dockerfile-linux.

Run Docker Image:
Run this command to execute the image:

docker run weather-app-backend