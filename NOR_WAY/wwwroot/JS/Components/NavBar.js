const template = document.createElement('template');
template.innerHTML = `
<style>
    img {
        padding-top: 2vh;
        margin-left: 2vw;
        height: 4vh;
    }

    ul li {
        list-style: none;
        display: inline;
        color: rgb(255,255,255);
        margin-left: 2vw;
        font-weight: bold;
        -webkit-font-smoothing: antialiased !important;
    }

    ul {
        position: absolute;
        top: 1vh;
        display: inline;
    }

    ul li a {
        -webkit-font-smoothing: antialiased !important;
        color: white;
        text-decoration: none;
        
    }

    ul li a:hover {
        text-decoration: none;
        color: rgb(190,190,190);
    }
</style>
<img 
    src="/Bilder/logo.svg" 
    alt="NOR-WAY sin logo. NOR i bold rødt og WAY i bold kursiv blått. 
    De to ordene skilles av en rød prikk"
/>
<ul>
    <li><a href="index.html">Kjøp Billett</a></li>
    <li>|</li>
    <li><a href="ruter.html">Ruter</a></li>
    <li>|</li>
    <li><a href="loggInn.html">Logg Inn</a></li>
</ul>`

class NavBar extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(template.content.cloneNode(true));
    }
}
window.customElements.define('nav-bar', NavBar);