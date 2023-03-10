## ExtratoClube.Konsi.Teste
O Desafio consiste em fazer uma API que busque e retorne a matrícula do servidor em um determinado portal. Foi desenvolvido um `crawler` para coletar esse dado no portal e uma API para fazer input e buscar o resultado depois.
### Pré-Requisitos
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker]( https://www.docker.com)
### Executando o projeto
Dentro da pasta `src` rodar o comando:
`docker-compose up --build`.
### Api
`http://localhost:8080/api/v1/extract/benefits`.
### Swagger
Acessar o caminho `http://localhost:8080/swagger`.
### Tecnologias
- .NET 6, C#
- Selenium Web Driver 4.8.1
