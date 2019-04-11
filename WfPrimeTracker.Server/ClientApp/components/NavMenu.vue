<template>
    <nav
        data-component="navmenu"
        class="navbar navbar-expand-md navbar-dark bg-dark"
    >
        <a class="navbar-brand" href="#">
            <img
                src="https://i.imgur.com/PQPrX2s.png"
                width="30"
                height="30"
                class="d-inline-block align-top"
                alt=""
            />
            Prime Tracker
        </a>
        <button
            class="navbar-toggler"
            type="button"
            data-toggle="collapse"
            data-target="#navbarSupportedContent"
            aria-controls="navbarSupportedContent"
            aria-expanded="false"
            aria-label="Toggle navigation"
        >
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <ul class="navbar-nav mr-auto">
                <li class="nav-item active">
                    <a class="nav-link" href="#">Tracker</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="#">Relics</a>
                </li>
            </ul>
            <form class="form-inline my-2 my-lg-0">
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text" id="basic-addon1">
                            <i class="fas fa-search"></i>
                        </span>
                    </div>
                    <input
                        type="text"
                        class="form-control"
                        placeholder="Search..."
                        aria-label="Username"
                        aria-describedby="basic-addon1"
                    />
                </div>
            </form>
        </div>
    </nav>
</template>