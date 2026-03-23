window.onload = async function(){
    const jogo = JSON.parse(sessionStorage.getItem('selectedGame'));

    await carregaCapa(jogo.appId)
    renderizaConquistas(jogo.conquistas)
}

async function carregaCapa(appId){

    const imageUrl = `https://cdn.akamai.steamstatic.com/steam/apps/${appId}/library_hero.jpg`;

    await new Promise((resolve, reject) =>{
        const img = new Image()
        img.src = imageUrl
        img.onload = resolve
        img.onerror = resolve // carega sem a imagem mesmo, para nao disparar um erro
    })
     /** @type {HTMLElement} */
    const container = document.querySelector('.capa');
    if(container)
        container.style.backgroundImage = `url('${imageUrl}')`
}

function renderizaConquistas(conquistas){
    
    const container = document.querySelector('.container-conquistas');
    const table = document.createElement('table');
    if (!container)
    {
        console.error("Erro: a classe não existe!!");
        return;
    }

    conquistas.forEach(c => {
    
        const tr = document.createElement('tr');
        tr.className = c.conquistada ? 'complete-achievement' : 'uncomplete-achievement';
        tr.innerHTML = `
            <td>${c.nome}</td>
            <td>${c.descricao}</td>
            <td>${c.conquistada ? '✔' : '✘'}</td>
        `;
        table.appendChild(tr);
    });
    container.appendChild(table);
}
