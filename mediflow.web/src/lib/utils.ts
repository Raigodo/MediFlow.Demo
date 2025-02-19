import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

export function cn(...inputs: ClassValue[]) {
	return twMerge(clsx(inputs));
}

export function setError(form: any, errors?: Record<string, string[]> | null) {
	if (!errors) return;
	Object.entries(errors).forEach(([key, value]) => {
		form.setError(key, {
			type: 'manual',
			message: value[0],
		});
	});
}
