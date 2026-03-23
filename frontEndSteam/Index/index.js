const botao = document.querySelector('#btn-steamid');
const input = document.querySelector('#input-steamid');

botao.addEventListener('click', function(event) {
    event.preventDefault();
    const steamIdText = input.value.trim();

    if (steamIdText)
    {
        console.log("entrei aq");
        window.location.href = `../Home/home.html?id=${steamIdText}`;
    }
    else
    {
        console.error("Digite um Steam ID valido");   
    }  
});
