<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<!--<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="fr"> -->

<!-- CURLD MUST BE ACTIVE ON WAMP SERVER IF YOU WORK IN LOCAL -->
<!-- CURLD MUST BE ACTIVE ON WAMP SERVER IF YOU WORK IN LOCAL -->
<!-- CURLD MUST BE ACTIVE ON WAMP SERVER IF YOU WORK IN LOCAL -->
<!-- CURLD MUST BE ACTIVE ON WAMP SERVER IF YOU WORK IN LOCAL -->

<html xmlns="http://www.w3.org/1999/xhtml" lang="fr"> 

	<head> <title>Test MIN Number</title>	<!-- Title Name -->
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" >
	</head>

	<body id="bodyIndex"> Test MIN Number <!-- Title Page -->
		<br><br>
		<div>	
			<form method="post" action="min.php?"  > <!-- Title Page -->
				<fieldset>
				 <legend>Man</legend> <!--Man area -->
					<br>
						<label id = "labelcontrol" >MIN Number</label>  <br> <!-- Execution of the page min.php after pressing the Test button -->
						<input type="text" id="ControlString" name="Man_IDCARD" size="20" 	   maxlength="40" value="" id="Man_IDCARD" > <!-- Entry box MIN number of man -->
					<br><br>
				</fieldset>
			   <br>
				<fieldset>
				 <legend>Woman</legend> <!--Woman area -->
					<br>
						<label id = "labelcontrol" >MIN Number</label> 			<br>
						<input type="text" id="ControlString" name="Woman_IDCARD" size="20"maxlength="40" value="" id="Woman_IDCARD" > <!-- Entry box MIN number of woman -->
					<br> <br>
				</fieldset>
				<fieldset >
				 <legend>Login and Password WDSF</legend>  <!--Login and passcord WDSF area -->
					<?php
					$login = ""; $mdp = ""; // initializing variables 
					if ((isset($_POST['Login']) == TRUE) and (isset($_POST['Password']) == TRUE) ) // if variables exist (they exist after pressing the Test button)
					{
						$login = $_POST['Login']; $mdp= $_POST['Password']; // Retrieving previous values
					}
					echo '	
						
					<br>
						<label id = "labelcontrol" >Login</label> 			<br>
						<input type="text" id="ControlString" name="Login" size="20"maxlength="40" value="'.$login.'" id="Login" >
					<br> 
						<label id = "labelcontrol" >PassWord</label> 			<br>
						<input type="text" id="ControlString" name="Password" size="20"maxlength="40" value="'.$mdp.'" id="Password" >					
					<br><br>
					<a href="http://projects.worlddancesport.org/wiki/display/PUB/Authentication">Authentication </a> 
					<br>
					<a href="https://www.worlddancesport.org/Account/Register">Register </a> 
					';
					
					 // Entry box Login WDSF with previous values if existing -
					// Entry box Poassword WDSF with previous values if existing -
					// Links for register
					?>
				</fieldset>				
				
				<br>
				<div id="champsBoutonEnrol">
					<input type="submit" value="Test" id="Test" > <!-- Test Button -->
				</div>
				<br>
				<br>
				<br>
			</form>
			<?php
			if ((isset($_POST['Man_IDCARD']) == TRUE) and (isset($_POST['Woman_IDCARD']) == TRUE) and (isset($_POST['Login']) == TRUE) and (isset($_POST['Password']) == TRUE) ) // if variables exist (they exist after pressing the Test button)
			{
				if ( (strlen ($_POST['Login']) > 0) and (strlen($_POST['Password']) > 0) ) // If Login and Password aren't not null
				{
					global  $W_FirstName, $M_FirstName, $Country, $W_Categorie, $M_Categorie;
					
					$M_IDCARD 	= $_POST['Man_IDCARD'];									 // Then retrieving MIN Number Man and Woman
					$W_IDCARD 	= $_POST['Woman_IDCARD'];			
					
					
					$ID_Couple_M = Test_Personne($_POST['Login'],$_POST['Password'],$M_IDCARD, "Male");				// Reading information about Man (Name and First Name, Category, sex, ID-COuple)
					$ID_Couple_W = Test_Personne($_POST['Login'],$_POST['Password'],$W_IDCARD, "Female");			// Reading information about Woman (Name and First Name, Category, sex, ID-COuple)		
																													//  Display all informations
					echo  'Man   ==> '.$M_FirstName.'   ('.$M_IDCARD.')     ;    '.$M_Categorie.'     ;    '.$ID_Couple_M.'<br>';
					echo  'Woman ==> '.$W_FirstName.'   ('.$W_IDCARD.')     ;    '.$W_Categorie.'     ;    '.$ID_Couple_W.'<br>';
					echo  'Country : '.$Country.'  <br>';
					
					if ($ID_Couple_W == $ID_Couple_M )
					{
						echo '<br><font style="background:#00FF00" >Couple valid';
					}
					else
					{
						echo '<br><font style="background:#FF0000">Couple not valid';
					}
				
				}					
				else
				{																									// If login and password aren't full  display message error
					echo '<br> Login and Password missing';
				}
			}
			?>
		</div>
	</body>
</html>

<!-- CURLD MUST BE ACTIVE ON WAMP SERVER IF YOU WORK IN LOCAL -->
<!-- CURLD MUST BE ACTIVE ON WAMP SERVER IF YOU WORK IN LOCAL -->
<!-- CURLD MUST BE ACTIVE ON WAMP SERVER IF YOU WORK IN LOCAL -->

<?php

function Test_Personne ($Login_Wdsf,$Password_Wdsf, $MIN_person, $Man_or_Woman)
{
	$Retour = -2;
	$mdp = $Login_Wdsf.':'.$Password_Wdsf;
	$mdp_code = base64_encode($mdp);																					// Encoding login and password
							
	$requete_Paypal = "https://services.worlddancesport.org/api/1/person?min=".$MIN_person;								// Construction of the first part of the application curl
			    
	$ch = curl_init($requete_Paypal);																					// Mise en forme poru l'execution de CURL
	curl_setopt($ch, CURLOPT_FRESH_CONNECT	, TRUE);																	// Set Attribute communication WDSF API via curl
	curl_setopt($ch, CURLOPT_SSL_VERIFYPEER	, 0);																		//  Set Attribute communication WDSF API via curl	
	curl_setopt($ch, CURLOPT_SSL_VERIFYHOST , 0);																		// Set Attribut ecommunication WDSF API via curl
	curl_setopt($ch, CURLOPT_RETURNTRANSFER	, 1);																		// Set Attribute communication WDSF API via curl

	//curl_setopt($ch, CURLOPT_PROXY, '100.254.100.20');
	curl_setopt($ch, CURLOPT_PROXY, 'clprox.bull.fr');																	// If nessesary use proxy 
	curl_setopt($ch, CURLOPT_PROXYPORT, 80);

	curl_setopt($ch, CURLOPT_HTTPHEADER		, array('Content-Type: application/vnd.worlddancesport.couples+xml','Accept: application/xml','Authorization: Basic '.$mdp_code));
	curl_setopt($ch, CURLOPT_HTTPAUTH		, CURLAUTH_BASIC);														// Set Attribute Authorization  WDSF API via curl
	curl_setopt($ch, CURLOPT_USERPWD		, $mdp_code);															// Set Attribute Authorization  WDSF API via curl
	curl_setopt($ch, CURLOPT_TIMEOUT		, 60);																	// Set Timeout before stop executionn  WDSF API via curl
	$resultat_paypal = curl_exec($ch);																				// Execute curl function to commuicate withe WDSF API
				
	
	if (!$resultat_paypal)																							// If there is a error
	{
		echo '<div id="Contenue_Erreur">'."Erreur curl".' : '.curl_error($ch).' <br></div>';						// Display error
	}
	else
	{			        							
		$Retour = Renvoie_MIN($resultat_paypal,$Man_or_Woman);														// Else read all information about this person 
	}
	curl_close($ch);																								// Close curl function
	
	return $Retour;
}



function Renvoie_MIN ($RequeteCurl,$ManWoman)																		
{
	$returnMIN = -1;
	$Path_MIN = "./Temp_MIN.xml"; 																					// Create  temporary xml file
	if (file_exists($Path_MIN) == TRUE)																				// IF Exists ?
	{
		unlink($Path_MIN);																							// Then delete it
	}	
	$fp = fopen($Path_MIN,"a+");																					//Open this same file
	fwrite($fp,$RequeteCurl);																						// Write all data receive by the WDSF API ($RequeteCurl)
	fclose($fp);																									// Close this file

	$dom = new DomDocument();																						// create a varaible to access xml file
	$dom->load($Path_MIN);																							// Load this file as a xml file
	$Textes =  $dom->getElementsByTagName("person");																// Get Elemet by Tag "Person" 
	
	if ($Textes->length > 0)   																						// Il at lesat one
	{																												// Then
		$returnMIN= $Textes->item(0)->getElementsByTagName("activeCoupleId")->item(0)->nodeValue;					//Get ActiveCoupleID 	
		$NameFirst= $Textes->item(0)->getElementsByTagName("name")->item(0)->nodeValue;								//Get name 	
		$GLOBALS["Country"]= $Textes->item(0)->getElementsByTagName("country")->item(0)->nodeValue;					// GetCountry
		$Categ = $Textes->item(0)->getElementsByTagName("activeCoupleAgeGroup")->item(0)->nodeValue;				// Get Age groupe
				
		//if ($Textes->item(0)->getElementsByTagName("sex")->item(0)->nodeValue == "Female") 
		if ($ManWoman == "Female")																					// If this person must have a Woman
		{ 
			$GLOBALS["W_FirstName"]= $NameFirst; 																	// Then 	storage of information to the variables woman
			$GLOBALS["W_Categorie"] = $Categ;
		}  
		else 
		{
			$GLOBALS["M_FirstName"] = $NameFirst;																	// Else 	storage of information to the variables man
			$GLOBALS["M_Categorie"] = $Categ;
		} 
		
		
		if ($ManWoman != $Textes->item(0)->getElementsByTagName("sex")->item(0)->nodeValue)							//We check that the person is expected sex
		{
			echo '<div id="Contenue_Erreur">Sex no valide :  Renvoie_MIN '.$ManWoman.'<br></div>'; 					//Else display message error
			$returnMIN = -10;																				
		}
	}
	
	return $returnMIN;
}

?>