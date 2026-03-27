# 🪙 CriptoBank

O **CriptoBank** é uma aplicação robusta de simulação de transações financeiras e investimentos em criptomoedas.

## 🚀 Tecnologias Utilizadas

- **Framework:** .NET 9 (C#)
- **Arquitetura:** Clean Architecture
- **Mensageria:** RabbitMQ
- **Banco de Dados:** PostgreSQL
- **ORM:** Entity Framework Core
- **Containerização:** Docker

---

## ⚙️ Configuração Importante

Antes de rodar o projeto, você deve configurar a conexão com o banco de dados:

1. **Docker:** Verifique o usuário e senha no arquivo `docker-compose.yml`.
2. **appsettings.json:** Na camada `CriptoBank.API` e `CriptoBank.Worker`, ajuste a `ConnectionStrings` para:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=CriptoBankDB;Username=admin;Password=yourpassword"
   }
   Nota: Se você alterou a senha no Docker, deve alterar aqui também.
   ```

## 🛠️ Como Executar

Siga o passo a passo abaixo para subir todo o ambiente (**Infraestrutura + Aplicação**):

---

### 🐳 1. Subir a Infraestrutura (Docker)

O projeto utiliza **Docker** para gerenciar o banco de dados e o sistema de mensageria.

Na raiz do projeto, execute:

```bash
docker-compose up -d
```

Isso irá subir os containers do:

- **PostgreSQL** (banco de dados)
- **RabbitMQ** (mensageria)

---

### 🗄️ 2. Aplicar as Migrations (Banco de Dados)

Com o PostgreSQL em execução, aplique as migrations para criar o esquema do banco:

```bash
dotnet ef database update --project CriptoBank.Infrastructure --startup-project CriptoBank.API
```

---

### 🚀 3. Rodar a Aplicação

Agora inicie os dois serviços principais:

#### ▶️ API

```bash
dotnet run --project CriptoBank.API
```

- A API ficará disponível em:

  ```
  http://localhost:5044
  ```

- Swagger:

  ```
  http://localhost:5044/swagger
  ```

---

#### ⚙️ Worker

```bash
dotnet run --project CriptoBank.Worker
```

O Worker ficará responsável por:

- Escutar as filas do **RabbitMQ**
- Processar transações de forma assíncrona
- Gerar histórico e notificações

---

### ✅ Fluxo de Inicialização

1. Subir containers com Docker
2. Aplicar migrations no banco
3. Iniciar a API
4. Iniciar o Worker

---

# 📖 Como Usar o Sistema

Após iniciar a API e o Worker, siga este fluxo básico no Swagger (`http://localhost:5044/swagger`) para simular a operação do banco:

---

## 🔐 1. Autenticação e Usuário

O sistema utiliza **Identity/JWT** para garantir a segurança das operações.

- Acesse o endpoint:

  - `POST /api/Auth/register` → Crie sua conta

- Em seguida:

  - `POST /api/Auth/login` → Faça login e obtenha seu token JWT

- No Swagger:

  - Clique em **Authorize** (no topo da página)
  - Cole o token para liberar o acesso aos endpoints protegidos

---

## 💰 2. Gestão de Saldo (Fiat)

Antes de investir em criptomoedas, é necessário ter saldo em BRL:

- Depositar saldo:

  - `POST /api/Wallet/deposito`
  - Exemplo de valor: `1000.00`

- Consultar saldo:

  - `GET /api/Wallet/saldo`

---

## 📈 3. Investindo em Criptomoedas

Com saldo disponível, você pode realizar operações de compra e venda:

- **Comprar criptomoeda:**

  - `POST /api/Wallet/buy`
  - Informe:

    - Nome da moeda (ex: `bitcoin`)
    - Valor em BRL

- **Vender criptomoeda:**

  - `POST /api/Wallet/sell`
  - Informe:
    - Nome da moeda (ex: `bitcoin`)
    - Quantidade da moeda que deseja vender

---

## 📊 4. Acompanhamento da Carteira

- Ver seus ativos e desempenho:

  - `GET /api/Holding/holdings`

- Histórico de transações:

  - `GET /api/Transaction/transactions`

> ℹ️ Cada transação realizada publica um evento no **RabbitMQ**, que é processado pelo Worker para gerar o histórico de operações.

---

## ✅ Fluxo Resumido

1. Criar conta
2. Fazer login e obter token
3. Depositar saldo em BRL
4. Comprar/vender criptomoedas
5. Acompanhar carteira e histórico

---

Desenvolvido por Lucas
