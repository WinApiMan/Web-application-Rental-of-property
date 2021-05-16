function longTermVisible() {
    let longTermAd = document.getElementById('longTermAdsSearching');
    let dailyAd = document.getElementById('dailyAdsSearching');

    if (longTermAd.style.display == 'none') {
        longTermAd.style.display = 'block';
        dailyAd.style.display = 'none';
    }
    else {
        longTermAd.style.display = 'none';
        dailyAd.style.display = 'none';
    }
}

function dailyVisible() {
    let longTermAd = document.getElementById('longTermAdsSearching');
    let dailyAd = document.getElementById('dailyAdsSearching');

    if (dailyAd.style.display == 'none') {
        dailyAd.style.display = 'block';
        longTermAd.style.display = 'none';
    }
    else {
        dailyAd.style.display = 'none';
        longTermAd.style.display = 'none';
    }
}
