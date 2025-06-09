# Desafio para .NET Pleno - Siemens Energy

Este documento descreve o exercício de programação para a vaga de .NET Pleno da Siemens Energy.

## Considerações sobre o desafio

* O prazo para entrega da solução é de 7 dias corridos, contados a partir da data de recebimento do desafio.
* Desenvolver uma API REST (.Net C#) que permita a criação, leitura, atualização e exclusão (CRUD) de gêneros, autores e livros.
* O desafio deve ser disponibilizado no GitHub do candidato, com instruções claras sobre como configurar e executar o projeto.

## Desafio

## 1. Regras de Negócio

* Um gênero pode possuir N livros.
* Um autor pode possuir N livros.
* Cada livro pertence a apenas um autor e um gênero.
 

## 2. Escolha do banco de dados (Obrigatório)

* SQL Server
* MySQL
* PostgreSQL

## 3. Backend (Obrigatório)

*Desenvolver uma API REST (.Net C#) que permita a criação, leitura, atualização e exclusão (CRUD) de gêneros, autores e livros.*

Requisitos Técnicos (Desejáveis):

* Boas práticas (exemplo: responsabilidade única, injeção de dependência, etc.)
* Versionamento da API (rotas)
* Documentação (exemplo: Swagger)
* Respostas padronizadas (HTTP Status Codes)
* Environments
* DTOs
* ViewModel
* Entidades
* ORM
* Migrations
* Testes de unidade

## 4. Configurando Projeto

## 🛠️ Tecnologias Utilizadas

- .NET 9
- ASP.NET Core Minimal API
- Entity Framework Core
- SQL Server
- Swagger
- xUnit (testes)

## 📦 Pré-requisitos

Certifique-se de ter instalado:

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- Banco de dados: SQL Server

### 4.1. Clonar o repositório

Abra o terminal da sua máquina e digite na seguinte ordem:

```bash
git clone https://github.com/pabelardo/minha-biblioteca-api.git
cd minha-biblioteca-api
```
### 4.2. Configurar o ambiente

No arquivo appsettings.Development.json ou no próprio appsettings.cs, configure a connection string do banco, trocando o que está entre chaves na propriedade DefaultConnection abaixo.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database={SUA DATABASE};User Id={SEU USUARIO};Password={SUA SENHA};"
  }
}
```

### 4.3 Aplicar migrations (opcional)

```bash
dotnet ef database update
```

Deixei um arquivo de script para criar as tabelas do banco na pasta  Data/Script/criar-tabelas.sql

## 5.Executar o projeto

```bash
dotnet run --project MinhaApi
```

