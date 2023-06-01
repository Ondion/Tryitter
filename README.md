# Tryitter - Projeto Final Aceleração em C# .NET

O Tryitter é uma rede social, totalmente baseada em texto. <br>
O objetivo deste projeto é proporcionar um ambiente em que pessoas estudantes podem, por meio de textos e imagens, compartilhar suas experiências e também acessar posts que possam contribuir para seu aprendizado.!

## :mag: Tecnologias utilizadas
- Construção da API - [ASP.NET ](https://dotnet.microsoft.com/pt-br/apps/aspnet)<br>
- Banco de dados [SQL Server ](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) <br>
 - Autenticação - [JWT](https://jwt.io/) <br> 
 - Testes - [Fluent Assertions](https://fluentassertions.com/) e [xUnit.net](https://xunit.net/) <br> 
 - Deploy - [Azure](https://azure.microsoft.com/pt-br/) <br>
## 📋 Execute o projeto em sua máquina

Clone o repositório:

```
git clone git@github.com:Ondion/Tryitter.git
cd src/triytter
dotnet restore
dotnet run
```
## 🕵 Diagrama UML da API <br>
![Tryitter drawio](https://github.com/Ondion/Tryitter/assets/65035109/3860ccf7-2e45-46f2-953b-e73000107130)

## 🧪 Executando os testes

Entre na pasta dos testes ```cd src/triytter.Test```

```
dotnet test
```

Testes de cobertura:

```

```
## :hammer: Deploy
>O deploy da aplicação foi executado utilizando o Microsoft Azure <br>
>Os links do deploy são:<br>
### Backend
`https://tryitter.azurewebsites.net/` 
### Banco de dados:
`https://tryitter.database.windows.net`


## 🔎 Documentação da API


<details>
<summary><strong> :runner: Students :runner:</strong></summary><br/>
 
```
  GET /Students 
```
 ```
  GET /Student/:id
```
```
  GET /Student/Name/
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `name` | `string` | **Obrigatório**.  Nome do estudante. |



```
  POST /Student
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `name` | `string` | **Obrigatório**.  Nome do estudante. |
| `email` | `string` | **Obrigatório**.  Email do estudante. |
| `Password` | `number` | **Obrigatório**.  Senha da conta criada. |
| `status` | `number` | **Obrigatório**.  status do estudante. |

```
  POST /Login 
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `email` | `string` | **Obrigatório**.  Seu email. |
| `password` | `string` | **Obrigatório**. Sua senha. |
| ` Authorization`      | `string` | **Obrigatório**. :key: Token do login deve ser passado no header. |

:exclamation::exclamation: Retorna Token 

```
  PATCH /Student/:id
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `name` | `string` | **Obrigatório**. : Nome do estudante. |
| `email` | `string` | **Obrigatório**. : Email do estudante. |
| `password` | `number` | **Obrigatório**. :Senha da conta criada. |
| `status` | `number` | **Obrigatório**.   status do estudante. |
| ` Authorization`      | `string` | **Obrigatório**. :key: Token do login deve ser passado no header. |

```
  DELETE/Student/:id
```
| `Authorization`      | `string` | **Obrigatório**. :key: Token do login deve ser passado no header. |
:exclamation::exclamation: Ao deletar um estudanten todos os seus post criados são deletados.<br>
</details>

<details>
<summary><strong> :page_facing_up: Posts :page_facing_up:</strong></summary><br/>


```
  GET /Post
```
```
  GET /Post/:id
```
```
  GET /Post/Student/:id
```
```
  GET /Post/Last/Student/:id
```
```
  GET /Post/StudentName
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `name` | `string` | **Obrigatório**.  Nome do estudante. |
```
  /Post/Last/StudentName
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `name` | `string` | **Obrigatório**.  Nome do estudante. |
```
  POST /Post
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `content` | `string` | **Obrigatório**. Texto da postagem. |
| `image` | `string` |  Imagem a ser postada |
| `studentEmail` | `number` | **Obrigatório**. Email do estudante |
| ` Authorization`      | `string` | **Obrigatório**. :key: Token do login deve ser passado no header. |

```
  PUT /Post/:id 
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `content` | `string` | **Obrigatório**. Texto da postagem. |
| `image` | `string` |  Imagem a ser postada |
| `studentEmail` | `number` |**Obrigatório**. Email do estudante |
| ` Authorization`      | `string` | **Obrigatório**. :key: Token do login deve ser passado no header. |

```
  DELETE /Post/:id 
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `studentEmail` | `number` | Email do estudante |
| ` Authorization`      | `string` | **Obrigatório**. :key: Token do login deve ser passado no header. |
 
</details>

## :pencil2: Projeto Executado por:

 Tamires Sousa [GitHub ](https://github.com/tamireshc)| [Linkedin](https://www.linkedin.com/in/tamires-s/)  <br>
 Fábio Xavier [GitHub ](https://github.com/Ondion) | [Linkedin ](https://www.linkedin.com/in/fabionxavier/) 
 



