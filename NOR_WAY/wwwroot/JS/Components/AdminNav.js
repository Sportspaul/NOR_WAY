const adminNav = document.createElement('template');
adminNav.innerHTML = `
<style>
   
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
        margin-bottom: 2vw;
    }

    ul li a {
        -webkit-font-smoothing: antialiased !important;
        color: black;
        text-decoration: none;
    }

    ul li a:hover {
        text-decoration: none;
        color: rgb(190,190,190);
    }
</style>

<ul>
    <li><a href="ruteoversikt.html">Ruteoversikt</a></li>
    <li><a href="ruteoversikt.html">Avganger</a></li>
    <li><a href="ruteoversikt.html">Billettyper</a></li>
    <li><a href="ruteoversikt.html">Ordre</a></li>
</ul>`

class AdminNav extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(adminNav.content.cloneNode(true));
    }
}
window.customElements.define('admin-nav', AdminNav);