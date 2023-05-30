# Tryitter - Projeto Final Aceleração em C# .NET

O Tryitter é uma rede social, totalmente baseada em texto. <br>
O objetivo deste projeto é proporcionar um ambiente em que pessoas estudantes podem, por meio de textos e imagens, compartilhar suas experiências e também acessar posts que possam contribuir para seu aprendizado.!

## 📋 Execute o projeto em sua máquina

Clone o repositório:

```
git clone git@github.com:Ondion/Tryitter.git
cd src/triytter
dotnet restore
dotnet run
```
## 🕵 Diagrama UML da API <br>
![Tryitter drawio (1)](https://github.com/Ondion/Tryitter/assets/65035109/fcd18d95-7a40-4cd5-bb9a-8596637a9581)

## 🔎 Documentação da API

### :runner: Students :runner:
```
  GET /Students
```
```
  GET /Student/Name/:name
```
```
  GET /Student/Id/:id
```
```
  POST /Student
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `name` | `string` | **Obrigatório**. :information_desk_person: Nome do estudante. |
| `email` | `string` | **Obrigatório**. :mailbox: Email do estudante. |
| `Password` | `number` | **Obrigatório**. :closed_lock_with_key: Senha da conta criada. |
| `status` | `number` | **Obrigatório**. :+1: status do estudante. |

```
  POST /Login 
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `email` | `string` | **Obrigatório**. :mailbox: Seu email. |
| `password` | `string` | **Obrigatório**.:closed_lock_with_key: Sua senha. |

:exclamation::exclamation: Retorna Token 

```
  PATCH /Student/:id
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `name` | `string` | **Obrigatório**. :information_desk_person: Nome do estudante. |
| `email` | `string` | **Obrigatório**. :mailbox: Email do estudante. |
| `password` | `number` | **Obrigatório**. :closed_lock_with_key:Senha da conta criada. |
| `status` | `number` | **Obrigatório**.  :+1: status do estudante. |
| ` Authorization`      | `string` | **Obrigatório**. :key: Token do login deve ser passado no header. |

```
  DELETE/Student/:id
```
| `Authorization`      | `string` | **Obrigatório**. :key: Token do login deve ser passado no header. |

##
### :page_facing_up: Posts :page_facing_up:


## 🧪 Executando os testes

Entre na pasta dos testes ```cd src/triytter.test```

```
dotnet test
```

Testes de cobertura:

```

```



