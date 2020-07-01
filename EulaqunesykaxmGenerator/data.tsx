export const vowels: Array<Vowel> = ['i', 'y', 'u', 'o', 'e', 'a'];
export const consonants: Array<Consonant> = [
    'p', 'fh', 'f', 't', 'c', 'x',
    'k', 'q', 'h', 'r', 'z', 'm',
    'n', 'r', 'l', 'j', 'w', 'b',
    'vh', 'v', 'd', 's', 'g', 'dz',
    'ph', 'ts', 'ch', 'ng', 'sh',
    'th', 'dh', 'kh', 'rkh', 'rl'
];
export const letters: Array<Letter> = [
    "i", "y", "u", "o", "e", "a",
    "p", "fh", "f", "t", "c", "x",
    "k", "q", "h", "r", "z", "m",
    "n", "r", "l", "j", "w", "b",
    "vh", "v", "d", "s", "g", "dz",
    "ph", "ts", "ch", "ng", "sh",
    "th", "dh", "kh", "rkh", "rl",
];
export type Vowel = 'i' | 'y' | 'u' | 'o' | 'e' | 'a';
export type Consonant =
    'p' | 'fh' | 'f' | 't' | 'c' | 'x' |
    'k' | 'q' | 'h' | 'r' | 'z' | 'm' |
    'n' | 'r' | 'l' | 'j' | 'w' | 'b' |
    'vh' | 'v' | 'd' | 's' | 'g' | 'dz' |
    'ph' | 'ts' | 'ch' | 'ng' | 'sh' |
    'th' | 'dh' | 'kh' | 'rkh' | 'rl';
export type Letter = Vowel | Consonant;
export interface SoundPair {
    left: Letter;
    right: Letter;
}
export interface ExampleDictionary {
    [id: string]: Array<string>;
}
export interface Example {
    word: string;
    parts: Array<string>;
    leftIndex: number;
    rightIndex: number;
}
