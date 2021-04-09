/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

// Le informazioni generali relative a un assembly sono controllate dal seguente set di attributi.
// Modificare i valori di questi attributi per modificare le informazioni associate a un assembly.

[assembly: AssemblyTitle("Clash Of SL Server")]
[assembly: AssemblyDescription("A .NET Clash Of SL Server Emulator")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Sky Production")]
[assembly: AssemblyProduct("Clash Of SL Server")]
[assembly: AssemblyCopyright("DARK")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Se si imposta ComVisible su false, i tipi in questo assembly non saranno visibili ai componenti
// COM. Se è necessario accedere a un tipo in questo assembly da COM, impostare su true l'attributo
// ComVisible per tale tipo.

[assembly: ComVisible(false)]

//Per iniziare la compilazione delle applicazioni localizzabili, impostare
//<UICulture>CultureYouAreCodingWith</UICulture> nel file .csproj
//all'interno di un <PropertyGroup>.  Ad esempio, se si utilizza l'inglese (Stati Uniti)
//nei file di origine, impostare <UICulture> su en-US.  Rimuovere quindi il commento dall'attributo
//NeutralResourceLanguage riportato di seguito.  Aggiornare "en-US" nella
//riga sottostante in modo che corrisponda all'impostazione UICulture nel file di progetto.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //dove si trovano i dizionari delle risorse specifiche del tema
                                     //(in uso se non è possibile trovare una risorsa nella pagina
                                     // oppure nei dizionari delle risorse dell'applicazione)
    ResourceDictionaryLocation.SourceAssembly //dove si trova il dizionario delle risorse generiche
                                              //(in uso se non è possibile trovare una risorsa nella pagina,
                                              // nell'applicazione o nei dizionari delle risorse specifiche del tema)
    )]

// Version information for an assembly consists of the following four values:
//
// Major Version Minor Version Build number Revision
//
// You can specify all the values or you can default to their numbers
// Revision and build using the asterisk '*' as shown below: [assembly: AssemblyVersion ("1.0. *")]

[assembly: AssemblyVersion("0.7.1.0")]
[assembly: AssemblyFileVersion("0.7.1.0")]
