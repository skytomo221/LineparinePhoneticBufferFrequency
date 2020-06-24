const vowels: Array<Vowel> = ['i', 'y', 'u', 'o', 'e', 'a'];
const consonants: Array<Consonant> = [
    'p', 'fh', 'f', 't', 'c', 'x',
    'k', 'q', 'h', 'r', 'z', 'm',
    'n', 'r', 'l', 'j', 'w', 'b',
    'vh', 'v', 'd', 's', 'g', 'dz',
    'ph', 'ts', 'ch', 'ng', 'sh',
    'th', 'dh', 'kh', 'rkh', 'rl'
];
type Vowel = 'i' | 'y' | 'u' | 'o' | 'e' | 'a';
type Consonant =
    'p' | 'fh' | 'f' | 't' | 'c' | 'x' |
    'k' | 'q' | 'h' | 'r' | 'z' | 'm' |
    'n' | 'r' | 'l' | 'j' | 'w' | 'b' |
    'vh' | 'v' | 'd' | 's' | 'g' | 'dz' |
    'ph' | 'ts' | 'ch' | 'ng' | 'sh' |
    'th' | 'dh' | 'kh' | 'rkh' | 'rl';
type Letter = Vowel | Consonant;
interface SoundPair {
    left: Letter;
    right: Letter;
}
interface ExampleDictionary {
    [id: string]: Array<string>;
}
