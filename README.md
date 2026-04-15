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

## 🛠️ Como Rodar o Projeto

Siga o passo a passo abaixo para subir todo o ambiente:

---

### 🐳 1. Pré-requisitos

Certifique-se de ter instalado:

Docker Desktop (que já inclui o Docker Compose).

Git.

### 🚀 3. Rodar a Aplicação

Clone o Repositório

```bash
git clone https://github.com/seu-usuario/CriptoBank.git
cd CriptoBank
```

Agora inicie o serviço :

```bash
docker-compose up --build -d
```

- A API ficará disponível em:

  ```
  http://localhost:5000
  ```

- Swagger:

  ```
  http://localhost:5000/swagger
  ```

---

O Worker ficará responsável por:

- Escutar as filas do **RabbitMQ**
- Processar transações de forma assíncrona
- Gerar histórico e notificações

---

### ✅ Fluxo de Inicialização

1. Subir containers com Docker
2. Iniciar o container

---

# 📖 Como Usar o Sistema

Após iniciar a API , siga este fluxo básico no Swagger (`http://localhost:5000/swagger`) para simular a operação do banco:

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
3. Fazer Sync das moedas - POST "api/Coin/sync"
4. Depositar saldo em BRL
5. Comprar/vender criptomoedas
6. Acompanhar carteira e histórico

---

Desenvolvido por Lucas
