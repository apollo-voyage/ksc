module.exports = {
    base: '/ksc/',
    title: 'ksc',
    description: 'ksc: The Kerboscript compiler for your command line.',
    head: [
        ['link', { rel: 'icon', href: '/images/logo.png' }]
    ],
    dest: '/docs/',
    serviceWorker: true,
    themeConfig: {
        logo: '/images/logo.png',
        nav: [
            {
                text: 'GitHub',
                link: 'https://github.com/mrbandler/ksc'
            }
        ],
        sidebar: {
            '/hub/': [
                {
                    title: 'ksc: The Kerboscript compiler',
                    collapsable: false,
                    children: [
                        '',
                        'init',
                        'compile',
                        'watch',
                        'run',
                        'deploy',
                    ]
                }
            ]
        }
    }
}