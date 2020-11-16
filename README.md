# clinica

Aplicativo desktop feito para uma pequena clinica.
Algumas funcionalidades são:

- Cadastro de pacientes;
- Cadastro de funcionários;
- Fila de espera para pacientes;
- Controle de prontuários de pacientes (necessidade do Microsoft Word);
- Criar orçamento;
- Administração de procedimentos laboratoriais;
- Administração de procedimentos de exames de imagem;
- Controle de histórico.

### Pendente

- Associar atendente com histórico de consulta e exames. Isso se torna necessário para saber qual funcionário atendeu e recebeu dinheiro de um paciente.

### Bugs conhecidos

- Raras vezes, exames de laboratório aparecem repetidos ou com nome trocados na lista de espera e histórico (Eu nunca consegui reproduzir esse bug, mas houveram alguns relatos);
- Caso o usuário não tenha o Microsoft Word instalado em sua máquina, o programa irá fechar sem aviso ao:
  - Abrir orçamento;
  - Abrir prontuário;
  - Após inserir um exame laboratorial na lista de espera.
