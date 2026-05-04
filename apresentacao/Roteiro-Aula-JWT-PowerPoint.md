# Roteiro de aula — Autenticação JWT em Web API (.NET)

**Disciplina:** Desenvolvimento Web com C#  
**Projeto de referência:** MinhaPrimeiraApi  
**Objetivo do roteiro:** servir de base para montar uma apresentação no PowerPoint (títulos, bullets e notas do professor).

**Duração sugerida:** 50–90 minutos (ajuste o número de slides conforme o tempo).

---

## Como usar este arquivo no PowerPoint

- Cada bloco **Slide N** vira um slide (ou dois, se o conteúdo for denso).
- **Título do slide:** linha em negrito após “Slide”.
- **Conteúdo:** bullets para o corpo do slide.
- **Notas do professor:** parágrafo(s) após “Notas do professor” — copie para o painel de notas no PowerPoint.
- **Demonstração ao vivo:** trechos marcados com “Demo:” — execute no Visual Studio / VS Code e no arquivo `MinhaPrimeiraApi.http`.

---

## Slide 1 — Capa

- **Título:** Autenticação JWT em ASP.NET Core Web API
- **Subtítulo:** Conceitos, fluxo e implementação no projeto MinhaPrimeiraApi
- **Rodapé (opcional):** Seu nome · instituição · data

**Notas do professor:** Apresentar o tema da aula e a expectativa: ao final, o aluno entende o que é um JWT, por que APIs usam Bearer tokens, e como proteger endpoints no .NET com `[Authorize]`.

---

## Slide 2 — Por que autenticar uma API?

- APIs REST são **stateless**: cada requisição deve trazer prova de identidade quando necessário.
- Sem autenticação, qualquer cliente poderia **criar, alterar ou apagar** dados.
- Objetivos: **identificar** o usuário (ou cliente), **autorizar** ações e **auditar** acessos.

**Notas do professor:** Contrastar com sessão em servidor (cookies em MVC tradicional). Em APIs modernas, token assinado é comum; JWT é um formato popular de token.

---

## Slide 3 — O que é JWT?

- **JWT** (JSON Web Token): string compacta com três partes em Base64Url: **Header.Payload.Signature**.
- **Header:** algoritmo (ex.: HS256).
- **Payload:** **claims** (ex.: identificador do usuário, expiração).
- **Signature:** garante **integridade** e **origem** (quem tem a chave secreta pode assinar).

**Notas do professor:** Enfatizar: o payload é codificado, não criptografado por padrão — não coloque segredos sensíveis em claims sem criptografia adicional. A assinatura impede adulteração.

---

## Slide 4 — Fluxo típico (login + Bearer)

1. Cliente envia **credenciais** (ex.: usuário e senha) para um endpoint de **login**.
2. Servidor valida e devolve um **JWT**.
3. Cliente envia o token no cabeçalho: `Authorization: Bearer <token>`.
4. API valida assinatura, emissor, audiência e validade; aplica **autorização** nas rotas.

**Notas do professor:** Desenhar na lousa ou usar animação: setas entre cliente, endpoint `/login` e endpoints protegidos. Mencionar HTTPS em produção para proteger credenciais e token em trânsito.

---

## Slide 5 — Pacotes e configuração no .NET

- Pacote: **Microsoft.AspNetCore.Authentication.JwtBearer** (alinhado à versão do ASP.NET Core do projeto).
- Em `Program.cs`: `AddAuthentication`, `AddJwtBearer`, `AddAuthorization`.
- Pipeline: `UseAuthentication()` e `UseAuthorization()` **antes** de `MapControllers()`.
- Parâmetros usuais: **chave simétrica** (HMAC), **Issuer**, **Audience**, tempo de vida do token.

**Notas do professor:** Mostrar no projeto onde a chave e issuer/audience são lidos de `appsettings.json`. Reforçar: em produção, chave via **User Secrets**, variável de ambiente ou cofre — nunca commitar segredo real.

---

## Slide 6 — Onde fica a “chave secreta”?

- **Jwt:SecretKey** deve ser longa o suficiente para **HMAC-SHA256** (ex.: ≥ 32 caracteres).
- **Jwt:Issuer** e **Jwt:Audience** alinham emissão e validação.
- **Jwt:ExpirationMinutes** define validade do token.

**Notas do professor:** Explicar ClockSkew (tolerância de relógio). No projeto, usuários são gravados na tabela **Usuarios** no MySQL com senha em hash; em produção evolua para **ASP.NET Identity** completo, OpenId Connect ou provedor de identidade gerenciado.

---

## Slide 7 — Emissão do token no projeto

- Serviço **`IJwtTokenService` / `JwtTokenService`**: monta claims (ex.: `sub`, `jti`), assina com a chave e retorna a string JWT.
- **`AuthController`**: `POST /api/Auth/register` persiste o usuário; `POST /api/Auth/login` valida `LoginRequest` no banco e retorna `LoginResponse` com o token.

**Notas do professor:** Abrir `JwtTokenService.cs` e mostrar `JwtSecurityToken` e `WriteToken`. Relacionar claims com o que aparece em [jwt.io](https://jwt.io) (somente para decodificar payload em aula — não expor chave).

---

## Slide 8 — Protegendo endpoints: `[Authorize]`

- Atributo **`[Authorize]`** em actions ou controllers: exige usuário autenticado (token válido).
- **`[AllowAnonymous]`** libera rota sem token (ex.: login).
- No projeto: **GET** de Produtos e Categorias permanecem públicos; **POST, PUT, PATCH, DELETE** exigem JWT.

**Notas do professor:** Discutir políticas e roles (`[Authorize(Roles = "Admin")]`) como próximo passo. Perguntar aos alunos: “Por que não protegemos GET?” — resposta pode envolver catálogo público vs. dados sensíveis.

---

## Slide 9 — Testando com `.http` ou Postman

- **Passo 1:** `POST /api/Auth/register` com JSON `{ "username": "...", "password": "..." }` (senha ≥ 6 caracteres). Se o usuário já existir, resposta **409 Conflict**.
- **Passo 2:** `POST /api/Auth/login` com o mesmo usuário e senha; copiar o `token` para `@token` em `MinhaPrimeiraApi.http`.
- **Passo 3:** chamar POST/PUT/DELETE com cabeçalho `Authorization: Bearer <token>`.
- **401 Unauthorized:** token ausente, inválido ou expirado.

**Demo:** Registrar, fazer login e chamar um POST protegido sem token (401) e com token (201).

**Notas do professor:** Mostrar erro 401 no cliente e, se possível, log de autenticação no console. Opcional: mostrar token expirado após alterar `ExpirationMinutes` para um valor baixo em Development.

---

## Slide 10 — Erros comuns e boas práticas

- Chave curta ou issuer/audience divergentes → validação falha.
- Esquecer `UseAuthentication` / `UseAuthorization` → `[Authorize]` não funciona como esperado.
- Armazenar segredos em repositório público.
- **Refresh tokens**, **revogação** e **Identity** não estão neste exemplo — são evolução natural.

**Notas do professor:** Reforçar checklist antes de avaliação: build passando, login retornando JWT, operação protegida só com Bearer.

---

## Slide 11 — Resumo e próximos tópicos

- JWT: formato, assinatura e envio como **Bearer**.
- .NET: JwtBearer, configuração, emissão e **`[Authorize]`**.
- Próximos temas sugeridos: **roles**, **policies**, **refresh token**, **OAuth2/OpenID Connect**, **Swagger com segurança**.

**Notas do professor:** Fechar com pergunta: “Quando trocar JWT por cookies ou por fluxo OAuth completo?” — breve discussão.

---

## Slide 12 — Referências rápidas

- Documentação: [ASP.NET Core — Autenticação JWT Bearer](https://learn.microsoft.com/aspnet/core/security/authentication/configure-jwt-bearer-authentication)
- RFC 7519 — JSON Web Token (JWT)
- Projeto: `Program.cs`, `JwtTokenService.cs`, `AuthController.cs`, `appsettings.json`

**Notas do professor:** Indicar onde está o roteiro (`apresentacao/Roteiro-Aula-JWT-PowerPoint.md`) e o arquivo de testes HTTP na raiz do projeto.

---

## Checklist para o professor (fora dos slides)

- [ ] `dotnet build` sem erros.
- [ ] MySQL/Docker disponível se for testar CRUD completo.
- [ ] Copiar token após login antes das requisições protegidas.
- [ ] Slides: revisar ortografia e alinhar à versão do .NET do projeto (ex.: net10.0).

---

*Fim do roteiro — personalize títulos, exemplos e tempo de demonstração conforme sua turma.*
