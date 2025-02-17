import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
	eslint: {
		ignoreDuringBuilds: true, // Suppresses ESLint errors during the build process
	},
	reactStrictMode: false,
	// output: 'standalone',
};

export default nextConfig;
