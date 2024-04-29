document.addEventListener('DOMContentLoaded', function() {
    const reel = document.querySelector('.carousel .reel');
    const articles = document.querySelectorAll('.carousel .reel article');
    const backwardButton = document.querySelector('.carousel .backward');
    const forwardButton = document.querySelector('.carousel .forward');

    let currentIndex = 0;
    const itemWidth = articles[0].offsetWidth;
    const itemCount = articles.length;

    backwardButton.addEventListener('click', function() {
        if (currentIndex > 0) {
            currentIndex--;
            reel.style.transform = `translateX(-${currentIndex * itemWidth}px)`;
    }
    });

forwardButton.addEventListener('click', function() {
    if (currentIndex < itemCount - 1) {
        currentIndex++;
        reel.style.transform = `translateX(-${currentIndex * itemWidth}px)`;
}
});
});