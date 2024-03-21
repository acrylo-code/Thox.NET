class GDPR {

    constructor() {
        //this.showStatus();
        //this.showContent();
        this.bindEvents();

        if (this.cookieStatus() === null) this.showGDPR();
    }

    bindEvents() {
        let buttonAccept = document.querySelector('.gdpr-consent__button--accept');
        let buttonReject = document.querySelector('.gdpr-consent__button--reject');
        buttonAccept.addEventListener('click', () => {
            this.cookieStatus('accept');
            // this.showStatus();
            this.showContent();
            this.hideGDPR();
        });
        buttonReject.addEventListener('click', () => {
            this.cookieStatus('reject');
            // this.showStatus();
            this.showContent();
            this.hideGDPR();
        });
    }

    showContent() {
        // this.resetContent();
        const status = this.cookieStatus() == null ? 'not-chosen' : this.cookieStatus();
        const element = document.querySelector(`.content-gdpr-${status}`);
        //element.classList.add('show');
    }

    // resetContent(){
    //     const classes = [
    //         '.content-gdpr-accept',
    //         '.content-gdpr-reject',
    //         '.content-gdpr-not-chosen'];

    //     for(const c of classes){
    //         document.querySelector(c).classList.add('hide');
    //         document.querySelector(c).classList.remove('show');
    //     }
    // }

    // showStatus() {
    //     document.getElementById('content-gpdr-consent-status').innerHTML =
    //         this.cookieStatus() == null ? 'Niet gekozen' : this.cookieStatus();
    // }

    cookieStatus(status) {

        if (status) {
            const currentTime = new Date();
            const hours = currentTime.getHours();
            const minutes = currentTime.getMinutes();
            const formattedTime = `${hours}:${minutes < 10 ? '0' : ''}${minutes}`; // Add leading zero if minutes is less than 10
            localStorage.setItem('gdpr-consent-choice', status);
            localStorage.setItem('gdpr-consent-metadata', JSON.stringify({
                datum: currentTime.getDate() + '-' + (currentTime.getMonth() + 1) + '-' + currentTime.getFullYear(),
                tijd: formattedTime
            }));
        }

        return localStorage.getItem('gdpr-consent-choice');
    }
    hideGDPR() {
        document.querySelector(`.gdpr_container`).classList.add('hide');
        document.querySelector(`.gdpr_container`).classList.remove('show');
    }

    showGDPR() {
        document.querySelector(`.gdpr_container`).classList.add('show');
    }
}

const gdpr = new GDPR();

