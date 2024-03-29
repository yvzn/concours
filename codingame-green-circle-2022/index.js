class Player {
	constructor(inputs) {
		this.playerLocation = parseInt(inputs[0]); // id of the zone in which the player is located
		this.playerScore = parseInt(inputs[1]);
		this.playerPermanentDailyRoutineCards = parseInt(inputs[2]); // number of DAILY_ROUTINE the player has played. It allows them to take cards from the adjacent zones
		this.playerPermanentArchitectureStudyCards = parseInt(inputs[3]);
	}
}

class Application {
	constructor(inputs) {
		this.objectType = inputs[0];
		this.id = parseInt(inputs[1]);
		this.trainingNeeded = parseInt(inputs[2]); // number of TRAINING skills needed to release this application
		this.codingNeeded = parseInt(inputs[3]); // number of CODING skills needed to release this application
		this.dailyRoutineNeeded = parseInt(inputs[4]); // number of DAILY_ROUTINE skills needed to release this application
		this.taskPrioritizationNeeded = parseInt(inputs[5]); // number of TASK_PRIORITIZATION skills needed to release this application
		this.architectureStudyNeeded = parseInt(inputs[6]); // number of ARCHITECTURE_STUDY skills needed to release this application
		this.continuousDeliveryNeeded = parseInt(inputs[7]); // number of CONTINUOUS_DELIVERY skills needed to release this application
		this.codeReviewNeeded = parseInt(inputs[8]); // number of CODE_REVIEW skills needed to release this application
		this.refactoringNeeded = parseInt(inputs[9]);
	}

	needed() {
		return [
			this.trainingNeeded,
			this.codingNeeded,
			this.dailyRoutineNeeded,
			this.taskPrioritizationNeeded,
			this.architectureStudyNeeded,
			this.continuousDeliveryNeeded,
			this.codeReviewNeeded,
		];
	}
}

class Card {
	constructor(inputs) {
		this.cardsLocation = inputs[0]; // the location of the card list. It can be HAND, DRAW, DISCARD or OPPONENT_CARDS (AUTOMATED and OPPONENT_AUTOMATED will appear in later leagues)
		this.trainingCardsCount = parseInt(inputs[1]);
		this.codingCardsCount = parseInt(inputs[2]);
		this.dailyRoutineCardsCount = parseInt(inputs[3]);
		this.taskPrioritizationCardsCount = parseInt(inputs[4]);
		this.architectureStudyCardsCount = parseInt(inputs[5]);
		this.continuousDeliveryCardsCount = parseInt(inputs[6]);
		this.codeReviewCardsCount = parseInt(inputs[7]);
		this.refactoringCardsCount = parseInt(inputs[8]);
		this.bonusCardsCount = parseInt(inputs[9]);
		this.technicalDebtCardsCount = parseInt(inputs[10]);
	}

	count() {
		return [
			this.trainingCardsCount,
			this.codingCardsCount,
			this.dailyRoutineCardsCount,
			this.taskPrioritizationCardsCount,
			this.architectureStudyCardsCount,
			this.continuousDeliveryCardsCount,
			this.codeReviewCardsCount,
		];
	}
}


class Move {
	constructor(inputs) {
		this.name = inputs[0];
		this.target = parseInt(inputs[1]);
	}

}

let debug = true,
	_readline = () => {
		// eslint-disable-next-line no-undef
		let entry = readline();
		if (debug) console.error(entry);
		return entry;
	};

while (true) {
	let gamePhase = _readline(),
		nApplications = parseInt(_readline()),
		applications = [...Array(nApplications)].map(() => new Application(_readline().split(' '))),
		players = [...Array(2)].map(() => new Player(_readline().split(' '))),
		nCardLocations = parseInt(_readline()),
		cardsLocations = [...Array(nCardLocations)].map(() => new Card(_readline().split(' '))),
		nPossibleMoves = parseInt(_readline()),
		possibleMoves = [...Array(nPossibleMoves)].map(() => new Move(_readline().split(' ')));

	if (gamePhase === 'MOVE') {

		let needed = applications
			.map(a => a.needed())
			.reduce(function sumArrayIndexByIndex(prev, next) {
				return prev.map((value, index) => value + next[index]);
			})

		let mostNeeded = needed
			.map((value, index) => ({ value, index }))
			.sort(function reverseNaturalOrder(a, b) {
				return b.value - a.value;
			})
			.map(a => a.index);

		let action = 'RANDOM';
		const isCloseToOpponent = (position) => {
			let positionOfOpponent = players[1].playerLocation;
			return position == positionOfOpponent;
		}

		for (let index of mostNeeded) {
			let match = possibleMoves.filter(move => move.name === 'MOVE' && move.target === index);
			if (match.length > 0 && !isCloseToOpponent(match.target)) {
				action = `${match[0].name} ${match[0].target}`;
				break;
			}
		}

		if (action === 'RANDOM') for (let index of mostNeeded) {
			let match = possibleMoves.filter(move => move.name === 'MOVE' && move.target === index);
			if (match.length > 0) {
				action = `${match[0].name} ${match[0].target}`;
				break;
			}
		}

		console.log(action);

	} else if (gamePhase === ' GIVE_CARD') {

		let needed = applications
			.map(a => a.needed())
			.reduce(function sumArrayIndexByIndex(prev, next) {
				return prev.map((value, index) => value + next[index]);
			})

		let leastNeeded = needed
			.map((value, index) => ({ value, index }))
			.sort()
			.map(a => a.index);

		let action = 'RANDOM';

		for (let index of leastNeeded) {
			let match = possibleMoves.filter(move => move.name === 'GIVE' && move.target === index);
			if (match.length > 0) {
				action = `${match[0].name} ${match[0].target}`;
				break;
			}
		}

		console.log(action);

	} else if (gamePhase === 'PLAY_CARD') {

		let hand = cardsLocations.filter(card => card.cardsLocation === 'HAND')[0]

		let action = 'WAIT';
		if (hand.trainingCardsCount > 0) {
			action = 'TRAINING'
		} else if (hand.architectureStudyCardsCount > 0) {
			action = 'ARCHITECTURE_STUDY'
		} else if (hand.codeReviewCardsCount > 0) {
			action = 'CODE_REVIEW'
		} else if (hand.refactoringCardsCount > 0 && players[0].technicalDebtCardsCount > 0) {
			action = 'REFACTORING'
		}

		console.log(action);

	} else if (gamePhase === 'RELEASE') {

		let hand = cardsLocations.filter(card => card.cardsLocation === 'HAND')[0]
		let handCount = hand.count();

		let releaseScore = applications
			.map(a => a.needed()
				.map(function penaltyIfNotInHand(value, index) {
					return handCount[index] - value;
				})
				.reduce(function sumArray(prev, next) {
					return prev + next;
				}));

		let bestRelease = releaseScore
			.map((value, index) => ({ value, index }))
			.sort(function reverseNaturalOrder(a, b) {
				return b.value - a.value;
			})
			.map(a => a.index);

		let action = 'RANDOM';

		for (let index of bestRelease) {
			let match = possibleMoves.filter(move => move.name === 'RELEASE' && move.target === index);
			if (match.length > 0) {
				action = `${match[0].name} ${match[0].target}`;
				break;
			}
		}

		console.log(action);

	} else {

		console.log('RANDOM');

	}

}
