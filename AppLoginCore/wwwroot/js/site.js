//Função JS para mascaras
$(document).ready(function () {
    $('.CEP').mask('00000-000');
    $('.dinheiro').mask('000.000.000.000.000,00', { reverse: true });
    $('.CPF').mask('000.000.000-00', { reverse: true });
    $('.Telefone').mask('(00) 0000-0000');
});
