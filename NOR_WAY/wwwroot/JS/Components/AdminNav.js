const adminNav = document.createElement("template");
adminNav.innerHTML = `
<style>

    ul li {
        list-style: none;
        display: inline;
        color: rgb(150,150,150);
        margin-right: 2vw;
        font-weight: bold;
        -webkit-font-smoothing: antialiased !important;
        margin-bottom: 0;
    }

    ul {
        padding: 0;
        margin-top: 0;
        margin-bottom: 3vh;
    }

    ul li a {
        -webkit-font-smoothing: antialiased !important;
        color: rgb(65,65,65);
        text-decoration: none;
    }

    ul li a:hover {
        text-decoration: none;
        color: rgb(0,0,0);
    }
</style>

<ul>
    <li><a href="ruter.html">Ruter</a></li>
    <li>|</li>
    <li><a href="billettyper.html">Billettyper</a></li>
    <li>|</li>
    <li><a href="stopp.html">Stopp</a></li>
    <li>|</li>
    <li><a href="ordre.html">Ordre</a></li>
</ul>`;

class AdminNav extends HTMLElement {
	constructor() {
		super();
		this.attachShadow({ mode: "open" });
		this.shadowRoot.appendChild(adminNav.content.cloneNode(true));
	}
}
window.customElements.define("admin-nav", AdminNav);
