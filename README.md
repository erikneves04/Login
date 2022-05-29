# Authorization & Authentication

<div align="justify"> 
  Olá, pessoas!

  Esse reporitório foi criado com o objetivo de eu ter contato com diferentes formas de realizar o controle de acesso aos endpoints de uma API ASP.NET. 
</div> 

## Metas:
- CRUD de usuários;
- Registro de acessos;
- Limitar tentativas de login;
- Validação dos dados(Email & Password);

## Design patterns usados:
- <a hrf="https://en.wikipedia.org/wiki/Service_layer_pattern">Service layer pattern;</a>
- <a hrf="https://deviq.com/design-patterns/repository-pattern">Repository pattern;</a>

<div align="justify"> 
  Obs. 2: Não apliquei o <a hrf="https://www.macoratti.net/16/01/net_uow1.htm"> Unity of Work </a> porque não gostei desse modelo de implementação, sendo que, do meu ponto  de vista, injetar algumas instâncias de IRepository é mais simples(e organizado) do que injetar somente IUnityOfWork para acessar todos os repositórios poteriormente...
</div> 

## Aprimoramento:
<div align="justify"> 
A branch 'Master' desse projeto referência a versão mais desenvolvida desse sistema. </br>
As demais versões podem ser acessadas em suas respectivas branch's.
</div> 

## Datas/Branchs:
- 03/05/2022 -> Versão 1.0: Inicial, feita sem o auxílio de serviços prontos;
