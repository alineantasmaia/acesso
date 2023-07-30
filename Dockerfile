#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bv.Acesso.Api/Bv.Acesso.Api.csproj", "Bv.Acesso.Api/"]
COPY ["Bv.Acesso.Dominio/Bv.Acesso.Dominio.csproj", "Bv.Acesso.Dominio/"]
COPY ["Bv.Acesso.Infra/Bv.Acesso.Infra.csproj", "Bv.Acesso.Infra/"]
RUN dotnet restore "Bv.Acesso.Api/Bv.Acesso.Api.csproj"
COPY . .
WORKDIR "/src/Bv.Acesso.Api"
RUN dotnet build "Bv.Acesso.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bv.Acesso.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bv.Acesso.Api.dll"]