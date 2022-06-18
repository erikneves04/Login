# Authorization & Authentication

<div align="justify"> 
  Olá, pessoas!

  Esse reporitório foi criado com o objetivo de eu ter contato com diferentes formas de realizar o controle de acesso aos endpoints de uma API ASP.NET. <br>
  É válido destacar que foquei em desenvolver o modelo de acessos e não busquei atender a outras funcionalidades como o tratamento de erros(usando um Middleware) para evitar a repetição de try/catch e a adição de paginação dos dados.
</div> 

## Metas:
- CRUD de usuários;
- Registro de acessos;
- Limitar tentativas de login;
- Validação dos dados(Email & Password - Formatação);

## Design patterns usados:
- <a hrf="https://en.wikipedia.org/wiki/Service_layer_pattern">Service layer pattern;</a>
- <a hrf="https://deviq.com/design-patterns/repository-pattern">Repository pattern;</a>

## Aprimoramento:
<div align="justify"> 
A branch 'Master' desse projeto referência a versão mais recente. </br>
As demais versões podem ser acessadas em suas respectivas branch's.
</div> 

## Datas/Branchs:
- 03/05/2022 -> Versão 1.0: Inicial, feita sem o auxílio de serviços prontos;
- 18/06/2022 -> Versão 2.0: Implementação baseada na utilização do JWT, mantendo as mesmas funcionalidades da anterior;
- dd/mm/2022 -> (NÃO-IMPLEMENTADA) Versão 3.0: Incremento da funcionalidade de login via certificado digital;
