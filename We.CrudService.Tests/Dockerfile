FROM microsoft/dotnet:2.2-sdk
WORKDIR /src
COPY . .
RUN dotnet restore

ENTRYPOINT ["dotnet", "test", "--results-directory:/TestResults/", "--logger:trx"]
