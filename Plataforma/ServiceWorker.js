﻿//var CACHE_NAME = 'static-v1';

//self.addEventListener('install', function (event) {
//    event.waitUntil(
//        caches.open(CACHE_NAME).then(function (cache) {
//            return cache.addAll([
//                '/',
//                '/index.html',
//                '/styles.css',
//                '/app.js',
//                '/manifest.js',
//                '/vendor.js',
//            ]);
//        })
//    )
//});

//self.addEventListener('activate', function activator(event) {
//    event.waitUntil(
//        caches.keys().then(function (keys) {
//            return Promise.all(keys
//                .filter(function (key) {
//                    return key.indexOf(CACHE_NAME) !== 0;
//                })
//                .map(function (key) {
//                    return caches.delete(key);
//                })
//            );
//        })
//    );
//});

//self.addEventListener('fetch', function (event) {
//    event.respondWith(
//        caches.match(event.request).then(function (cachedResponse) {
//            return cachedResponse || fetch(event.request);
//        })
//    );
//});

// This is the "Offline page" service worker

//const CACHE = "pwabuilder-page";

//// TODO: replace the following with the correct offline fallback page i.e.: const offlineFallbackPage = "offline.html";
//const offlineFallbackPage = "ToDo-replace-this-name.html";

//// Install stage sets up the offline page in the cache and opens a new cache
//self.addEventListener("install", function (event) {
//    console.log("[PWA Builder] Install Event processing");

//    event.waitUntil(
//        caches.open(CACHE).then(function (cache) {
//            console.log("[PWA Builder] Cached offline page during install");

//            if (offlineFallbackPage === "ToDo-replace-this-name.html") {
//                return cache.add(new Response("TODO: Update the value of the offlineFallbackPage constant in the serviceworker."));
//            }

//            return cache.add(offlineFallbackPage);
//        })
//    );
//});

//// If any fetch fails, it will show the offline page.
//self.addEventListener("fetch", function (event) {
//    if (event.request.method !== "GET") return;

//    event.respondWith(
//        fetch(event.request).catch(function (error) {
//            // The following validates that the request was for a navigation to a new document
//            if (
//                event.request.destination !== "document" ||
//                event.request.mode !== "navigate"
//            ) {
//                return;
//            }

//            console.error("[PWA Builder] Network request Failed. Serving offline page " + error);
//            return caches.open(CACHE).then(function (cache) {
//                return cache.match(offlineFallbackPage);
//            });
//        })
//    );
//});

//// This is an event that can be fired from your page to tell the SW to update the offline page
//self.addEventListener("refreshOffline", function () {
//    const offlinePageRequest = new Request(offlineFallbackPage);

//    return fetch(offlineFallbackPage).then(function (response) {
//        return caches.open(CACHE).then(function (cache) {
//            console.log("[PWA Builder] Offline page updated from refreshOffline event: " + response.url);
//            return cache.put(offlinePageRequest, response);
//        });
//    });
//});


// This is the service worker with the combined offline experience (Offline page + Offline copy of pages)

const CACHE = "pwabuilder-offline-page";

// TODO: replace the following with the correct offline fallback page i.e.: const offlineFallbackPage = "offline.html";
const offlineFallbackPage = "ToDo-replace-this-name.html";

// Install stage sets up the offline page in the cache and opens a new cache
self.addEventListener("install", function (event) {
    console.log("[PWA Builder] Install Event processing");

    event.waitUntil(
        caches.open(CACHE).then(function (cache) {
            console.log("[PWA Builder] Cached offline page during install");

            if (offlineFallbackPage === "ToDo-replace-this-name.html") {
                return cache.add(new Response("TODO: Update the value of the offlineFallbackPage constant in the serviceworker."));
            }

            return cache.add(offlineFallbackPage);
        })
    );
});

// If any fetch fails, it will look for the request in the cache and serve it from there first
self.addEventListener("fetch", function (event) {
    if (event.request.method !== "GET") return;

    event.respondWith(
        fetch(event.request)
            .then(function (response) {
                console.log("[PWA Builder] add page to offline cache: " + response.url);

                // If request was success, add or update it in the cache
                event.waitUntil(updateCache(event.request, response.clone()));

                return response;
            })
            .catch(function (error) {
                console.log("[PWA Builder] Network request Failed. Serving content from cache: " + error);
                return fromCache(event.request);
            })
    );
});

function fromCache(request) {
    // Check to see if you have it in the cache
    // Return response
    // If not in the cache, then return the offline page
    return caches.open(CACHE).then(function (cache) {
        return cache.match(request).then(function (matching) {
            if (!matching || matching.status === 404) {
                // The following validates that the request was for a navigation to a new document
                if (request.destination !== "document" || request.mode !== "navigate") {
                    return Promise.reject("no-match");
                }

                return cache.match(offlineFallbackPage);
            }

            return matching;
        });
    });
}

function updateCache(request, response) {
    return caches.open(CACHE).then(function (cache) {
        return cache.put(request, response);
    });
}