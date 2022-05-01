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
- Implementação de <a hrf="https://pt.wikipedia.org/wiki/Teste_de_unidade">testes unitários</a>

## Design patterns usados:
- <a hrf="https://en.wikipedia.org/wiki/Service_layer_pattern">Service layer pattern;</a>
- <a hrf="https://deviq.com/design-patterns/repository-pattern">Repository pattern;</a>

<div align="justify"> 
  Obs. 1: Esses patterns vão ser alterados nas próximas versões, pois quero ter contato com outros modelos de projeto... Caso tudo ocorra bem, na versão 2.0(que usa a autenticação via JWT) já usará outros.</br></br>
  Obs. 2: Não apliquei o <a hrf="https://www.macoratti.net/16/01/net_uow1.htm"> Unity of Work </a> porque não gostei desse modelo de implementação, sendo que, do meu ponto  de vista, injetar algumas instâncias de IRepository é mais simples(e organizado) do que injetar somente IUnityOfWork para acessar todos os repositórios poteriormente...
</div> 

## Aprimoramento:
<div align="justify"> 
A branch 'Master' desse projeto referência a versão mais desenvolvida desse sistema. </br>
As demais versões podem ser acessadas em suas respectivas branch's.
</div> 

## Datas/Branchs:
- dd/mm/aaaa -> Versão x.0: description;
