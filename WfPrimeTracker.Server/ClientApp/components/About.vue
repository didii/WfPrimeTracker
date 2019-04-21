<template>
    <div data-component="about" class="container">
        <div class="jumbotron">
            <h1 class="display-4">Warframe Prime Tracker</h1>
            <p class="lead">
                A simple app that allows you to track what prime items you have in Warframe.
            </p>
            <hr class="my-4" />
            <p>
                This site is in no way affiliated with <a href="https://digitalextremes.com/" target="_blank">Digitals Extremes</a>, the company making the game <a href="https://www.warframe.com/" target="_blank">Warframe</a>. All data comes from the <a href="https://warframe.fandom.com/wiki/WARFRAME_Wiki" target="_blank">wikipedia</a> on <a href="https://www.fandom.com/" target="_blank">Fandom</a> which this site is also in no way affiliated with.
            </p>
            <p>
                If you want to take a look at the code, it's on <a href="https://github.com/didii/WfPrimeTracker" target="_blank"><i class="fab fa-github"></i> Github</a>. The backend is .Net Core with a Vue.js frontend.
            </p>
            <p>
                If you want to know more about the developer of this site, here are some more links.
                <ul>
                    <li>
                        <a href="https://github.com/didii" target="_blank"><i class="fab fa-github"></i> Github</a>
                    </li>
                    <li>
                        <a href="https://www.linkedin.com/in/dieter-van-broeck-476092142/" target="_blank"><i class="fab fa-linkedin"></i> LinkedIn</a>
                    </li>
                </ul>
            </p>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class About extends Vue {

}
</script>