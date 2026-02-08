# Company Rate Frontend

Ui for rating companies.

![GitHub top language](https://img.shields.io/github/languages/top/cccaaannn/CompanyRatingFrontend?color=blue&style=flat-square) ![GitHub repo size](https://img.shields.io/github/repo-size/cccaaannn/CompanyRatingFrontend?color=orange&style=flat-square) [![GitHub](https://img.shields.io/github/license/cccaaannn/CompanyRatingFrontend?color=green&style=flat-square)](https://github.com/cccaaannn/CompanyRatingFrontend/blob/master/LICENSE)

---

## Development

### Setup
1. Install dependencies
    ```shell
    dotnet restore
    ```
2. Create and update `appsettings.Development.json`
    ```shell
   cp appsettings.json appsettings.Development.json
    ```
3. Run
    ```shell
    dotnet run
    ```
4. Open [http://localhost:5215](http://localhost:5215) in your browser

### Docker

1. Build
    ```shell
    docker build -t company-rate-frontend .
    ```
2. Run
    ```shell
    docker run -d -p 8080:80 --name frontend company-rating-frontend
    ```

