let selectFilter = document.getElementById('ordem');
let filtroAtual = 'Defaut';
let steamid = null

window.onload = async function() {
    const urlId = new URLSearchParams(window.location.search);
    steamid = urlId.get('id')

    if (steamid) {
        await carregaTelaHome(steamid)
    } else {
        window.location.href = '../Index/index.html';
    }
};

selectFilter.addEventListener('change', async ()=>{
    filtroAtual = selectFilter.value;
    console.log("event");
    const perfil = await RetornaLista(steamid);
    if(perfil)
    {
        renderizaJogos(perfil);
    }
});

function filtraDados (listaJogos){
    switch(filtroAtual)
    {
        case 'horas_desc':
            listaJogos.jogos.sort((a, b) => b.gameHours - a.gameHours)
            break
        case 'horas_asc':
            listaJogos.jogos.sort((a, b) => a.gameHours - b.gameHours)
            break
        case 'nome_asc':
            listaJogos.jogos.sort((a, b) => a.name.localeCompare(b.name))
            break
        case 'nome_desc':
            listaJogos.jogos.sort((a, b) => b.name.localeCompare(a.name))
            break
        default:
            break
        
    } 
        console.log("oioioioioi",listaJogos);  

    return listaJogos
}

async function RetornaLista(steamid)
{
    if(!steamid)
    {
        console.error("Nenhum steamID encontrado");
        return;
    }

    try {
        const respostaBack = await fetch(`http://localhost:5000/api/perfil/${steamid}`);
        if(respostaBack.ok)
        {
            let listaJogos = await respostaBack.json();
            if(filtroAtual !== 'Defaut')
                listaJogos = filtraDados(listaJogos)
            console.log("RetornaLista:",listaJogos);
            return listaJogos;
        }

    } catch (error) {
        console.error("Erro ao conectar com o servidor");
    }
}

function renderizaJogos(perfil)
{
    const listaJogos = perfil.jogos
    const container = document.querySelector('.container-card');
    if (!container) 
    {
        console.error("Erro: a classe não existe!!");
        return;
    }

    container.innerHTML = '';

    listaJogos.forEach(jogo => {

        const total = jogo.conquistas.length;
        const conquistadas = jogo.conquistas.filter(c => c.conquistada === true).length;

        const card = document.createElement('article');
        card.className = conquistadas === total ? 'cards-complete' : 'cards'
        card.innerHTML = `
        <img class="img-card" src="https://cdn.akamai.steamstatic.com/steam/apps/${jogo.appId}/header.jpg" alt="${jogo.name}">
        <h3>${jogo.name}</h3>
        <p class="horas"><i class="fa-regular fa-clock"></i> ${jogo.gameHours} horas</p>
        <progress class ="${ conquistadas === total ? 'progress-complete' : 'progress-underway'}"value="${conquistadas}" max="${total}"></progress>
        <p class="achievements-count">${conquistadas}/${total}</p>
        `;

        card.addEventListener('click', ()=> {
            sessionStorage.setItem('selectedGame', JSON.stringify(jogo));// estou fazendo isso para evitar outra requisicao ao back
                                                                         // ja que ja tenho tudo nesse json atraves do usuario

            window.location.href = `../Details/details.html?id=${jogo.appId}&jogoid=${jogo.id}`;
        });
        container.appendChild(card);
    });
}

function ExibeUsuario(listaJogos){

 const containerUser = document.querySelector('.title-finder');
    if(!containerUser)
    {
        console.error("Erro: a classe não existe!");
        return;
    }

    containerUser.innerHTML= '';

    const cardHTML = `
    <div class="user-profile">
        <img class="img-profile" src="${listaJogos.avatarUrl}" alt="${listaJogos.name}">
        <h1>${listaJogos.name}</h1> 
    </div>
    `;

    containerUser.innerHTML += cardHTML;
} 

async function colocaCapaHead()
{
    /** @type {HTMLElement} */
    const capa = document.querySelector('.head-index');

    const imageUrl = `https://cdn.akamai.steamstatic.com/steam/apps/1091500/library_hero.jpg`;
    
    await new Promise((resolve, reject) =>{
        const img = new Image()
        img.src = imageUrl
        img.onload = resolve
        img.onerror = reject
    })

    if(capa)
    {
        capa.style.backgroundImage = `url('${imageUrl}')`
        capa.style.backgroundSize = 'cover';
        capa.style.backgroundPosition = 'center';
        capa.style.backgroundRepeat = 'no-repeat';
    }     
} 

async function carregaTelaHome(steamid)
{
    document.body.style.visibility = 'hidden'

    const perfil = await RetornaLista(steamid);

    await colocaCapaHead();
    if(perfil)
    {
        ExibeUsuario(perfil);
        renderizaJogos(perfil);
    }

    document.body.style.visibility = 'visible'
}