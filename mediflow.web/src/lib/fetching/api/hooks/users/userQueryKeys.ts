export const userQueryKeys = {
	all: ['users'],
	one: (req: { userId: string }) => ['users', `user:${req.userId}`],
	currentUser: ['users', 'user:current'],
	currentAvatar: ['users', 'user-avatar:current'],
};
