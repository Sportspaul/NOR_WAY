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
<a href="../index.html">
    <img
        src="../Bilder/logo.svg"
        alt="NOR-WAY sin logo. NOR i bold rødt og WAY i bold kursiv blått.
        De to ordene skilles av en rød prikk"
    />
</a>
<ul>
    <li><a href="../index.html">Kjøp Billett</a></li>
    <li>|</li>
    <li><a href="../ruter.html">Ruter</a></li>
    <li>|</li>
    <li><slot name="innlogging"></slot></li>
    <li><slot name="skille"></slot></li>
    <li><slot name="admin"></slot></li>
</ul>`

class NavBar extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(template.content.cloneNode(true));
    }
}
window.customElements.define('nav-bar', NavBar);