# Windows Forms Login

Este projeto � um simples programa de login e registro criado em C#.

## Depend�ncias

- MySql.Data.MySqlClient

## Funcionalidades

- Conex�o com banco de dados MySQL.
- Verifica��o de campos de e-mail e senha.
- Sistema de encripta��o SHA256.

## Encripta��o SHA256

```csharp
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
```

## Imagens

<p float="left">
  <img src="https://imgur.com/OzuXU64.png" width="160" />
  <img src="https://imgur.com/ZjC4pXE.png" width="160" /> 
  <img src="https://imgur.com/AMRP4zs.png" width="160" />
  <img src="https://imgur.com/GrB14cw.png" width="160" /> 
  <img src="https://imgur.com/CjYIXzT.png" width="160" />
</p>

## Uso

Antes de tudo, � preciso criar as tabelas, e voc� pode usar um [programa]([https://github.com/rodriaum/Login?tab=MIT-1-ov-file#MIT-1-ov-file](https://github.com/rodriaum/SQLTaskApplication)) que eu fiz dedicado para esse projeto. Ao iniciar o programa, uma conex�o com o banco de dados � iniciada. Quando o bot�o de login � clicado, o programa verifica se os campos de e-mail e senha est�o preenchidos. Se estiverem, ele cria um login n�o verificado no banco de dados.
Al�m da funcionalidade de login, o programa possui uma �rea de registro onde os novos usu�rios podem criar suas pr�prias contas, fornecendo suas credenciais de login.
Na �rea privada, existe uma p�gina para remover ou alterar a password de um e-mail a escolha registrado no banco de dados.

## Contribui��o

Contribui��es s�o bem-vindas. Por favor, fa�a um fork do projeto e crie um pull request com suas altera��es.

## Licen�a

Este projeto � licenciado sob a [MIT License](https://github.com/rodriaum/Login?tab=MIT-1-ov-file#MIT-1-ov-file).